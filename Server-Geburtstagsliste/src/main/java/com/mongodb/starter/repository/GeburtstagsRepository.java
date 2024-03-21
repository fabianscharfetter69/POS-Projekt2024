package com.mongodb.starter.repository;

import com.mongodb.starter.Geburtstag;
import com.mongodb.starter.GeburtstagDTO;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface GeburtstagsRepository {

    Geburtstag save(Geburtstag geburtstagEntity);

    List<Geburtstag> findAll();

    long delete(String id);

    void saveAll(List<GeburtstagDTO> geburtstagDTOs);
}
