package com.example.demo.Formatter;

import com.example.demo.Base.User;
import org.springframework.format.Formatter;
import java.text.ParseException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Locale;

public class UserFormatter implements Formatter<User> {
    @Override
    public String print(User user, Locale locale) {
        String result = user.getUserName();
        return result;
    }

    @Override
    public User parse(String s, Locale locale) throws ParseException {
        User user = new User();
       // user.setName(s);
      //  user.setId("id-"+s);
        DateTimeFormatter sdf = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss" );
        String time = LocalDateTime.now().format(sdf);
       // user.setTime(time);
        return user;
    }
}
