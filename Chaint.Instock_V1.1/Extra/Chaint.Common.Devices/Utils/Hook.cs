
//Referenced from Developer Express Inc.

using System;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Chaint.Common.Devices.Utils
{
	public interface IHookController {
		IntPtr OwnerHandle { get; }
		bool InternalPreFilterMessage(int Msg, Control wnd, IntPtr HWnd, IntPtr WParam, IntPtr LParam);
		bool InternalPostFilterMessage(int Msg, Control wnd, IntPtr HWnd, IntPtr WParam, IntPtr LParam);
	}
	public interface IHookController2 : IHookController {
		void WndGetMessage(ref Message msg);
	}
	public enum HookResult { Unknown, NotProcessed, Processed, ProcessedExit }
	public interface IHookControllerWithResult : IHookController {
		HookResult Result { get; set; }
	}
	public delegate int Hook(int ncode, IntPtr wParam, IntPtr lParam);
	public class HookInfo {
		List<IHookController> hookControllers;
		int threadId;
		public bool inHook, inMouseHook;
		internal HookManager.CWPSTRUCT hookStr = new HookManager.CWPSTRUCT();
		internal HookManager.MOUSEHOOKSTRUCTEX hookStrEx = new HookManager.MOUSEHOOKSTRUCTEX();
		public IntPtr wndHookHandle, getMessageHookHandle, mouseHookHandle;
		public Hook wndHookProc, mouseHookProc, getMessageHookProc;
		HookManager manager;
		public HookInfo(HookManager manager) {
			this.manager = manager;
			this.inMouseHook = false;
			this.inHook = false;
			wndHookHandle = getMessageHookHandle = mouseHookHandle = IntPtr.Zero;
			wndHookProc = mouseHookProc = getMessageHookProc = null;
			this.threadId = HookManager.GetCurrentThreadId();
			this.hookControllers = new List<IHookController>();
		}
		public List<IHookController> HookControllers { get { return hookControllers; } }
		public int ThreadId { get { return threadId; } }
		public Point GetPoint() {
			if(hookStrEx != null) {
				HookManager.POINT pt = hookStrEx.Mouse.Pt;
				return new Point(pt.X, pt.Y);
			}
			return Point.Empty;
		}
	}
	public class HookManager {
		Dictionary<int, HookInfo> hookHash;
		public List<IHookController> HookControllers;
		static HookManager defaultManager = new HookManager();
		public static HookManager DefaultManager { get { return defaultManager; } }
		public HookManager() {
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			Application.ThreadExit += new EventHandler(OnThreadExit);
			hookHash = new Dictionary<int, HookInfo>();
			HookControllers = new List<IHookController>();
		}
		~HookManager() {
			RemoveHooks();
			Application.ApplicationExit -= new EventHandler(OnApplicationExit);
			Application.ThreadExit -= new EventHandler(OnThreadExit);
		}
		public Dictionary<int, HookInfo> HookHash { get { return hookHash; } }
		public void CheckController(IHookController ctrl) {
			HookInfo hInfo = GetInfoByThread();
			if(hInfo.HookControllers.Contains(ctrl)) return;
			AddController(ctrl);
		}
		public void AddController(IHookController ctrl) {
			HookInfo hInfo = GetInfoByThread();
			hInfo.HookControllers.Add(ctrl);
			if(hInfo.HookControllers.Count == 1) InstallHook(hInfo);
		}
		public void RemoveController(IHookController ctrl) {
			HookInfo hInfo = GetInfoByThread();
			hInfo.HookControllers.Remove(ctrl);
			if(hInfo.HookControllers.Count == 0) RemoveHook(hInfo, false);
		}
		protected virtual HookInfo GetInfoByThread() {
			int thId = CurrentThread;
			HookInfo hInfo;
			lock(HookHash) {
				if(!HookHash.TryGetValue(thId, out hInfo)) {
					hInfo = new HookInfo(this);
					HookHash.Add(thId, hInfo);
				}
			}
			return hInfo;
		}
		public static int CurrentThread { get { return GetCurrentThreadId(); } }
		internal void InstallHook(HookInfo hInfo) {
			if(hInfo.wndHookHandle != IntPtr.Zero) return;
			hInfo.mouseHookProc = new Hook(MouseHook);
			hInfo.wndHookProc = new Hook(WndHook);
			hInfo.getMessageHookProc = new Hook(GetMessageHook);
			hInfo.wndHookHandle = SetWindowsHookEx(4, hInfo.wndHookProc, 0, hInfo.ThreadId); 
			hInfo.mouseHookHandle = SetWindowsHookEx(7, hInfo.mouseHookProc, 0, hInfo.ThreadId); 
			hInfo.getMessageHookHandle = IntPtr.Zero;
		}
		internal void RemoveHook(HookInfo hInfo, bool disposing) {
			lock(HookHash) {
				if(hInfo != null && hInfo.wndHookHandle != IntPtr.Zero) {
					UnhookWindowsHookEx(hInfo.wndHookHandle);
					hInfo.wndHookHandle = IntPtr.Zero;
					hInfo.wndHookProc = null;
					hInfo.getMessageHookHandle = IntPtr.Zero;
					hInfo.getMessageHookProc = null;
					UnhookWindowsHookEx(hInfo.mouseHookHandle);
					hInfo.mouseHookHandle = IntPtr.Zero;
					hInfo.mouseHookProc = null;
					HookHash.Remove(hInfo.ThreadId);
				}
			}
		}
		private void OnThreadExit(object sender, EventArgs e) {
			RemoveHook(GetInfoByThread(), true);
		}
		private void OnApplicationExit(object sender, EventArgs e) {
			Application.ThreadExit -= new EventHandler(OnThreadExit);
			Application.ApplicationExit -= new EventHandler(OnApplicationExit);
			RemoveHooks();
		}
		protected virtual void RemoveHooks() {
			lock(HookHash) {
				List<HookInfo> list = new List<HookInfo>(HookHash.Values);
				HookHash.Clear();
				for(int n = 0; n < list.Count; n++) {
					RemoveHook(list[n], true);
				}
			}
		}
		[DllImport("kernel32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
		public static extern int GetCurrentThreadId();
		[StructLayout(LayoutKind.Sequential)]
		internal sealed class CWPSTRUCT { 
			public IntPtr lParam; 
			public IntPtr wParam; 
			public int	message; 
			public IntPtr	hwnd; 
		}
		[StructLayout(LayoutKind.Sequential)]
		internal struct CWPRETSTRUCT { 
			public IntPtr lResult;
			public IntPtr lParam; 
			public IntPtr wParam; 
			public int	message; 
			public IntPtr	hwnd; 
		}
		[StructLayout(LayoutKind.Sequential)]
		internal struct POINT { 
			public int X;
			public int Y;
		}
		[StructLayout(LayoutKind.Sequential)]
		internal struct MOUSEHOOKSTRUCT { 
			public POINT	 Pt; 
			public IntPtr	 hwnd; 
			public uint		 wHitTestCode; 
			public IntPtr	 dwExtraInfo; 
		}
		[StructLayout(LayoutKind.Sequential)]
		internal class MOUSEHOOKSTRUCTEX {
			public MOUSEHOOKSTRUCT Mouse;
			public int mouseData;
		}
		[StructLayout(LayoutKind.Sequential)]
		internal struct API_MSG {
			public IntPtr   Hwnd; 
			public int   Msg; 
			public IntPtr WParam; 
			public IntPtr LParam; 
			public int  Time; 
			public POINT  Pt; 
			public Message ToMessage() {
				System.Windows.Forms.Message res = new System.Windows.Forms.Message();
				res.HWnd = this.Hwnd;
				res.Msg = this.Msg;
				res.WParam = this.WParam;
				res.LParam = this.LParam;
				return res;
			}
			public void FromMessage(ref Message msg) {
				this.Hwnd = msg.HWnd;
				this.Msg = msg.Msg;
				this.WParam = msg.WParam;
				this.LParam = msg.LParam;
			}
		}
		protected int WndHook(int ncode, IntPtr wParam, IntPtr lParam) {
			HookInfo hInfo = GetInfoByThread();
			int res = 0;
			if(!hInfo.inHook && lParam != IntPtr.Zero) {
				CWPSTRUCT hookStr = hInfo.hookStr;
				Marshal.PtrToStructure(lParam, hookStr);
				Control ctrl = null;
				try {
					try {
						ctrl = Control.FromHandle(hookStr.hwnd);
						hInfo.inHook = true;
						res = InternalPreFilterMessage(hInfo, hookStr.message, ctrl, hookStr.hwnd, hookStr.wParam, hookStr.lParam) ? 1 : 0;
					} finally {
						hInfo.inHook = false;
					}
					return CallNextHookEx(hInfo.wndHookHandle, ncode, wParam, lParam);
				} finally {
					InternalPostFilterMessage(hInfo, hookStr.message, ctrl, hookStr.hwnd, hookStr.wParam, hookStr.lParam);
				}
			} else
				return CallNextHookEx(hInfo.wndHookHandle, ncode, wParam, lParam);
		}
		protected int GetMessageHook(int ncode, IntPtr wParam, IntPtr lParam) {
			HookInfo hInfo = GetInfoByThread();
			if(!hInfo.inHook && lParam != IntPtr.Zero) {
				try {
					hInfo.inHook = true;
					API_MSG hookStr = (API_MSG)Marshal.PtrToStructure(lParam, typeof(API_MSG));
					InternalGetMessage(ref hookStr);
				} finally {
					hInfo.inHook = false;
				}
			} 
			return CallNextHookEx(hInfo.wndHookHandle, ncode, wParam, lParam); 
		}
		protected int MouseHook(int ncode, IntPtr wParam, IntPtr lParam) {
			HookInfo hInfo = GetInfoByThread();
			int res = 0;
			bool allowFutureProcess = true;
			if(ncode == 0) {
				if(!hInfo.inMouseHook && lParam != IntPtr.Zero) {
					try {
						MOUSEHOOKSTRUCTEX hookStrEx = hInfo.hookStrEx;
						Marshal.PtrToStructure(lParam, hookStrEx);
						MOUSEHOOKSTRUCT hookStr = hookStrEx.Mouse;
						Control ctrl = Control.FromHandle(hookStr.hwnd);
						hInfo.inMouseHook = true;
						allowFutureProcess = !InternalPreFilterMessage(hInfo, wParam.ToInt32(), ctrl, hookStr.hwnd, new IntPtr(hookStrEx.mouseData), new IntPtr((hookStr.Pt.Y << 16) | hookStr.Pt.X));
					} finally {
						hInfo.inMouseHook = false;
					}
				} else return CallNextHookEx(hInfo.mouseHookHandle, ncode, wParam, lParam);
			}
			res = CallNextHookEx(hInfo.mouseHookHandle, ncode, wParam, lParam); 
			if(!allowFutureProcess) res = -1;
			return res;
		}
		[DllImport("USER32.dll", CharSet=CharSet.Auto)]
		protected static extern IntPtr SetWindowsHookEx(int idHook, Hook lpfn, int hMod,int dwThreadId);
		[DllImport("USER32.dll", CharSet=CharSet.Auto)]
		protected static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,  IntPtr lParam);
		[DllImport("USER32.dll", CharSet=CharSet.Auto)]
		protected static extern bool UnhookWindowsHookEx(IntPtr hhk);
		internal bool InternalPreFilterMessage(HookInfo hInfo, int Msg, Control wnd, IntPtr HWnd, IntPtr WParam, IntPtr LParam) {
			bool result = false;
			for(int n = hInfo.HookControllers.Count - 1; n >= 0; n--) {
				if(n >= hInfo.HookControllers.Count) continue;
				IHookController ctrl = hInfo.HookControllers[n];
				result |= ctrl.InternalPreFilterMessage(Msg, wnd, HWnd, WParam, LParam);
				IHookControllerWithResult ctrl3 = ctrl as IHookControllerWithResult;
				if(ctrl3 != null && ctrl3.Result == HookResult.ProcessedExit)
					break;
			}
			return result;
		}
		internal bool InternalPostFilterMessage(HookInfo hInfo, int Msg, Control wnd, IntPtr HWnd, IntPtr WParam, IntPtr LParam) {
			bool result = false;
			for(int n = hInfo.HookControllers.Count - 1; n >=0 ; n --) {
				IHookController ctrl = hInfo.HookControllers[n];
				result |= ctrl.InternalPostFilterMessage(Msg, wnd, HWnd, WParam, LParam);
				if(Msg == 0x2) { 
					if(ctrl.OwnerHandle == HWnd) {
						RemoveController(ctrl);
					}
				}
				IHookControllerWithResult ctrl3 = ctrl as IHookControllerWithResult;
				if(ctrl3 != null && ctrl3.Result == HookResult.ProcessedExit)
					break;
			}
			return result;
		}
		internal void InternalGetMessage(ref API_MSG msg) {
			HookInfo hInfo = GetInfoByThread();
			for(int n = 0; n < hInfo.HookControllers.Count; n ++) {
				IHookController2 ctrl = hInfo.HookControllers[n] as IHookController2;
				if(ctrl != null) {
					Message m = msg.ToMessage();
					ctrl.WndGetMessage(ref m);
					msg.FromMessage(ref m);
				}
			}
		}
	}
	public delegate void MsgEventHandler(object sender, ref Message msg);
	public class ControlWndHookInfo {
		ControlWndHook hook;
		int refCount;
		public ControlWndHookInfo(ControlWndHook hook, MsgEventHandler handler, MsgEventHandler afterHandler) {
			this.refCount = 0;
			this.hook = hook;
			AddRef(handler, afterHandler);
		}
		public int RefCount { get { return refCount; } }
		public void AddRef(MsgEventHandler handler, MsgEventHandler afterHandler) {
			this.refCount ++; 
			if(Hook != null) {
				if(handler != null) Hook.WndMessage += handler;
				if(afterHandler != null) Hook.AfterWndMessage += afterHandler;
			}
		}
		public void Release(MsgEventHandler handler, MsgEventHandler afterHandler) {
			if(Hook != null) {
				if(handler != null) Hook.WndMessage -= handler;
				if(afterHandler != null) Hook.AfterWndMessage -= afterHandler;
			}
			if(-- this.refCount == 0) {
				if(this.hook != null) ReleaseCore();
			}
		}
		public ControlWndHook Hook { get { return hook; } }
		protected void ReleaseCore() {
			this.hook.Control = null;
			this.hook = null;
		}
	}
	public class ControlWndHook {
		static Hashtable hooks = new Hashtable();
		public static void AddHook(Control ctrl, MsgEventHandler handler, MsgEventHandler afterHandler) {
			ControlWndHookInfo info = hooks[ctrl] as ControlWndHookInfo;
			if(info == null) {
				ControlWndHook hook = new ControlWndHook();
				hook.Control = ctrl;
				info = new ControlWndHookInfo(hook, handler, afterHandler);
				hooks[ctrl] = info;
			} else
				info.AddRef(handler, afterHandler);
		}
		public static void RemoveHook(Control ctrl, MsgEventHandler handler, MsgEventHandler afterHandler) {
			ControlWndHookInfo info = hooks[ctrl] as ControlWndHookInfo;
			if(info == null) return;
			info.Release(handler, afterHandler);
			if(info.RefCount == 0) hooks.Remove(ctrl);
		}
		public event MsgEventHandler WndMessage;
		public event MsgEventHandler AfterWndMessage;
		Control control;
		MyCallBack wndProc = null;
		const int GWL_WNDPROC= -4;
		IntPtr prevProc;
		public ControlWndHook() {
			this.control = null;
			this.prevProc = IntPtr.Zero;
		}
		public Control Control {
			get { return control; }
			set {
				if(Control == value) return;
				if(Control != null) {
					UnHook();
				}
				this.control = value;
				if(Control != null) Hook();
			}
		}
		public virtual void UnHook() { UnHook(true); }
		protected virtual void UnHook(bool unsubscribeEvents) {
			if(Control == null) return;
			if(Control.IsHandleCreated) {
				if(prevProc != IntPtr.Zero) { 
					SetWindowLong(new HandleRef(this, Control.Handle), GWL_WNDPROC, prevProc);
				}
			}
			this.wndProc = null;
			this.prevProc = IntPtr.Zero;
			if(unsubscribeEvents) {
				Control.HandleDestroyed -= new EventHandler(OnControl_HandleDestroyed);
				Control.HandleCreated -= new EventHandler(OnControl_HandleCreated);
			}
		}
		public virtual void Hook() {
			UnHook();
			if(Control == null) return;
			HookCore();
			Control.HandleDestroyed += new EventHandler(OnControl_HandleDestroyed);
			Control.HandleCreated += new EventHandler(OnControl_HandleCreated);
		}
		protected virtual void HookCore() {
			if(Control != null && Control.IsHandleCreated) {
				this.wndProc = new MyCallBack(WindowProc); 
				this.prevProc = SetWindowLong2(new HandleRef(this, Control.Handle), GWL_WNDPROC, wndProc);
			}
		}
		protected virtual void OnControl_HandleDestroyed(object sender, EventArgs e) {
			UnHook(false);
		}
		protected virtual void OnControl_HandleCreated(object sender, EventArgs e) {
			Hook();
		}
		delegate IntPtr MyCallBack(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
		[DllImport("User32.dll", EntryPoint="SetWindowLong", CharSet=CharSet.Auto)]
		static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, IntPtr newLong);
		[DllImport("User32.dll", EntryPoint="SetWindowLong", CharSet=CharSet.Auto)]
		static extern IntPtr SetWindowLong2(HandleRef hWnd, int nIndex, MyCallBack newLong);
		[DllImport("User32.dll", EntryPoint="GetWindowLong", CharSet=CharSet.Auto)]
		static extern MyCallBack GetWindowLong(HandleRef hWnd, int nIndex);
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		static extern IntPtr CallWindowProc(IntPtr pPrevProc, IntPtr hWnd, int message,IntPtr wParam,IntPtr lParam);
		public IntPtr WindowProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam) {
			Message msg = new Message();
			msg.HWnd = hWnd;
			msg.WParam = wParam;
			msg.LParam = lParam;
			msg.Msg = message;
			IntPtr proc = this.prevProc;
			if(WndMessage != null) WndMessage(this, ref msg);
			if(proc == IntPtr.Zero) return IntPtr.Zero;
			IntPtr res = CallWindowProc(proc, msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
			if(AfterWndMessage != null) AfterWndMessage(this, ref msg);
			return res;
		}
	}
	public class FormHook {
	}
}
