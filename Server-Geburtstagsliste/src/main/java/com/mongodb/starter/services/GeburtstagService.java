package com.mongodb.starter.services;

import com.mongodb.starter.dtos.GeburtstagDTO;

import java.util.List;

public interface GeburtstagService {
    GeburtstagDTO save(GeburtstagDTO geburtstagDTO);
    List<GeburtstagDTO> saveAll(List<GeburtstagDTO> geburtstagEntities);
}
