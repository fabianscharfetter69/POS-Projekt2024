package com.mongodb.starter;

import org.bson.types.ObjectId;


public class Geburtstag {
    private ObjectId id;
    private String name;
    private String day;
    private String month;
    private String year;

    public Geburtstag(){

    }

    public Geburtstag(ObjectId id, String name, String day, String month) {
        this.id = id;
        this.name = name;
        this.day = day;
        this.month = month;
    }

    public Geburtstag(ObjectId id, String name, String day, String month, String year) {
        this.id = id;
        this.name = name;
        this.day = day;
        this.month = month;
        this.year = year;
    }

    public ObjectId getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getDay() {
        return day;
    }

    public String getMonth() {
        return month;
    }

    public String getYear() {
        return year;
    }

    public void setId(ObjectId id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setDay(String day) {
        this.day = day;
    }

    public void setMonth(String month) {
        this.month = month;
    }

    public void setYear(String year) {
        this.year = year;
    }

    @Override
    public String toString() {
        if (year == null) {
            return day + "." + month + ".\t" + name;
        } else {
            return day + "." + month + ".\t" + name + ", " + year;
        }
    }
}
