package com.mongodb.starter;

import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class GeburtstagServiceImpl implements GeburtstagService {

    private final GeburtstagsRepository geburtstagRepository;

    public GeburtstagServiceImpl(GeburtstagsRepository geburtstagRepository) {
        this.geburtstagRepository = geburtstagRepository;
    }

    @Override
    public GeburtstagDTO save(GeburtstagDTO geburtstagDTO) {
        return new GeburtstagDTO(geburtstagRepository.save(geburtstagDTO.toGeburtstagEntity()));
    }

    @Override
    public List<GeburtstagDTO> findAll() {
        return geburtstagRepository.findAll().stream()
                .map(GeburtstagDTO::new)
                .collect(Collectors.toList());
    }

    @Override
    public long delete(String id) {
        return geburtstagRepository.delete(id);
    }

    @Override
    public void addGeburtstage(List<GeburtstagDTO> geburtstagDTOs) {
        geburtstagRepository.saveAll(geburtstagDTOs);
    }
}
