package com.mongodb.starter;

import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface GeburtstagsRepository {

    Geburtstag save(Geburtstag geburtstagEntity);

    List<Geburtstag> findAll();

    long delete(String id);

    void saveAll(List<GeburtstagDTO> geburtstagDTOs);
}
