package com.mongodb.starter.services;

import com.mongodb.starter.dtos.GeburtstagDTO;
import com.mongodb.starter.models.Geburtstag;
import com.mongodb.starter.repositories.GeburtstagRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class GeburtstagServiceImpl implements GeburtstagService {

    private final GeburtstagRepository geburtstagRepository;

    public GeburtstagServiceImpl(GeburtstagRepository geburtstagRepository) {
        this.geburtstagRepository = geburtstagRepository;
    }

    @Override
    public GeburtstagDTO save(GeburtstagDTO carDTO) {
        return new GeburtstagDTO(geburtstagRepository.save(carDTO.toCarEntity()));
    }

    @Override
    public List<GeburtstagDTO> saveAll(List<GeburtstagDTO> carDTOs) {
        return carDTOs.stream()
                .map(Geburtstag::toGeburtstagEntity)
                .peek(geburtstagRepository::save)
                .map(Geburtstag::new)
                .toList();
    }

}
