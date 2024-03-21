package com.mongodb.starter;

import com.mongodb.starter.service.GeburtstagService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/geb-liste")
public class GeburtstagController {

    private final static Logger LOGGER = LoggerFactory.getLogger(GeburtstagController.class);
    private final GeburtstagService geburtstagService;

    public GeburtstagController(GeburtstagService geburtstagService) {
        this.geburtstagService = geburtstagService;
    }

    @PostMapping("/geburtstag")
    @ResponseStatus(HttpStatus.CREATED)
    public GeburtstagDTO addGeburtstag(@RequestBody GeburtstagDTO geburtstagDTO) {
        return geburtstagService.save(geburtstagDTO);
    }

    @PostMapping("/geburtstage")
    @ResponseStatus(HttpStatus.CREATED)
    public void addGeburtstage(@RequestBody List<GeburtstagDTO> geburtstagDTOs) {
        geburtstagService.addGeburtstage(geburtstagDTOs);
    }

    @GetMapping("/geburtstage")
    public List<GeburtstagDTO> findAllGeburtstage() {
        return geburtstagService.findAll();
    }

    @DeleteMapping("/geburtstag/{id}")
    public Long deleteGeburtstag(@PathVariable String id) {
        return geburtstagService.delete(id);
    }

    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }
}
