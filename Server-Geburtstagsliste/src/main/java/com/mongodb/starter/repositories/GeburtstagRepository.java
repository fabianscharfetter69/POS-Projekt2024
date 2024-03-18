package com.mongodb.starter.repositories;

import com.mongodb.starter.models.Geburtstag;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface GeburtstagRepository {
    Geburtstag save(Geburtstag geburtstag);
    List<Geburtstag> saveAll(List<Geburtstag> carEntities);
}
