package com.example.demo.Formatter;

import com.alibaba.fastjson.parser.DefaultJSONParser;
import com.alibaba.fastjson.parser.deserializer.ObjectDeserializer;

import java.lang.reflect.Type;
import java.util.Date;

public class TimestampValueDeserializer implements ObjectDeserializer {

    @Override
    public Date deserialze(DefaultJSONParser parser, Type type, Object fieldName) {
        long timestamp = parser.getLexer().longValue();
        return new Date(timestamp);

    }

    @Override
    public int getFastMatchToken() {
        return 0;
    }

}