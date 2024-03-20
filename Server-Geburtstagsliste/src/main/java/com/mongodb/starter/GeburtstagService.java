package com.mongodb.starter;

import java.util.List;

public interface GeburtstagService {

    GeburtstagDTO save(GeburtstagDTO geburtstagDTO);

    List<GeburtstagDTO> findAll();

    long delete(String id);

    void addGeburtstage(List<GeburtstagDTO> geburtstagDTOs);
}
