package com.example.demo.Mapper;
import com.example.demo.Base.User;
import org.apache.ibatis.annotations.Param;

import java.util.List;

public interface UserMapper {
    User selectUser(Integer id);
    List<User> selectUsers(@Param("UserName")String UserName);
    Void updateUser(Integer UserId);
    Void insertUser(User user);
    Integer deleteUser(Integer id);
}
