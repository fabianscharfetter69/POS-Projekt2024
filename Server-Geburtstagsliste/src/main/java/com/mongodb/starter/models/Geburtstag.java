package com.mongodb.starter.models;

public class Geburtstag {

    private String name;
    private String day;
    private String month;
    private String year;

    public Geburtstag(String name, String day, String month) {
        this.name = name;
        this.day = day;
        this.month = month;
    }

    public Geburtstag(String name, String day, String month, String year) {
        this.name = name;
        this.day = day;
        this.month = month;
        this.year = year;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDay() {
        return day;
    }

    public void setDay(String day) {
        this.day = day;
    }

    public String getMonth() {
        return month;
    }

    public void setMonth(String month) {
        this.month = month;
    }

    public String getYear() {
        return year;
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
