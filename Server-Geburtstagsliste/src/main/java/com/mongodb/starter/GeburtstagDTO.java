package com.mongodb.starter;

import org.bson.types.ObjectId;

public record GeburtstagDTO(
        String id,
        String name,
        String day,
        String month,
        String year) {

    public GeburtstagDTO(Geburtstag geburtstag) {
        this(geburtstag.getId() == null ? new ObjectId().toHexString() : geburtstag.getId().toHexString(),
                geburtstag.getName(), geburtstag.getDay(), geburtstag.getMonth(), geburtstag.getYear());
    }

    public Geburtstag toGeburtstagEntity() {
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new Geburtstag(_id, name, day, month, year);
    }
}
