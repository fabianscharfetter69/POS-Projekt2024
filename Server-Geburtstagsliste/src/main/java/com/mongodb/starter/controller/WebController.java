package com.mongodb.starter.controller;

import com.mongodb.starter.Geburtstag;
import com.mongodb.starter.GeburtstagDTO;
import com.mongodb.starter.service.GeburtstagService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.client.RestTemplate;

@Controller
public class WebController {
    private final RestTemplate restTemplate;

    public WebController(RestTemplate restTemplate) {
        this.restTemplate = restTemplate;
    }

    @GetMapping("/website")
    public String mainpage(Model model) {
        return "index";
    }

    @PostMapping("/newGeburtstag")
    public String newGeburtstag(@RequestParam("nameInput") String name, @RequestParam("dateInput") String dateString){

        System.out.println("Geburtstagskind: " + name);
        System.out.println("Datum: " + dateString);
        String[] arrDate = dateString.split("-");

        try {
            Geburtstag geb = new Geburtstag(null, name, arrDate[2], arrDate[1], arrDate[0]);
            GeburtstagDTO geburtstagDTO = new GeburtstagDTO(geb);

            // Senden der Daten an den GeburtstagController
            ResponseEntity<GeburtstagDTO> response = restTemplate.postForEntity("http://localhost:8081/geb-liste/geburtstag", geburtstagDTO, GeburtstagDTO.class);
            if (response.getStatusCode().is2xxSuccessful()) {
                System.out.println("Geburtstag erfolgreich hinzugefügt: " + response.getBody());
            } else {
                System.out.println("Fehler beim Hinzufügen des Geburtstags: " + response.getStatusCodeValue());
            }

        } catch (Exception e) {
            System.out.println(e.getMessage());
            // Handle parse error
        }

        return "redirect:/website";
    }
}
