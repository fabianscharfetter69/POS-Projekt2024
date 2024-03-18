package com.mongodb.starter.dtos;

import com.mongodb.starter.models.Geburtstag;

public record GeburtstagDTO(String name, String day, String month, String year) {

    public GeburtstagDTO(Geburtstag geburtstag) {
        this(geburtstag.getName(), geburtstag.getDay(), geburtstag.getMonth(), geburtstag.getYear());
    }

    public Geburtstag toGeburtstag() {
        return new Geburtstag(name, day, month, year);
    }
}
