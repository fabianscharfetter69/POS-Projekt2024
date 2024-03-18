package com.mongodb.starter.controllers;

import com.mongodb.starter.dtos.GeburtstagDTO;
import com.mongodb.starter.services.GeburtstagService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/geburtstags-liste")
public class GeburtstagsController {
    private final static Logger LOGGER = LoggerFactory.getLogger(GeburtstagsController.class);
    private final GeburtstagService geburtstagService;

    public GeburtstagsController(GeburtstagService geburtstagService) {

        this.geburtstagService = geburtstagService;
    }

    @PostMapping("geb")
    @ResponseStatus(HttpStatus.CREATED)
    public GeburtstagDTO postCar(@RequestBody GeburtstagDTO geburtstagDTO) {
        return geburtstagService.save(geburtstagDTO);
    }

    @PostMapping("gebs")
    @ResponseStatus(HttpStatus.CREATED)
    public List<GeburtstagDTO> postCars(@RequestBody List<GeburtstagDTO> geburtstagDTOS) {
        return geburtstagService.saveAll(geburtstagDTOS);
    }

    @GetMapping("gebs")
    public List<GeburtstagDTO> getCars() {
        return geburtstagService.findAll();
    }



    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }

}
