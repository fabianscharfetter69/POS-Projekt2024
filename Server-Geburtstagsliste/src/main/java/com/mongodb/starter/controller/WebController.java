package com.mongodb.starter.controller;

import com.mongodb.starter.Geburtstag;
import com.mongodb.starter.GeburtstagDTO;
import com.mongodb.starter.service.GeburtstagService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class WebController {

    private final GeburtstagService geburtstagService;

    public WebController(GeburtstagService geburtstagService) {
        this.geburtstagService = geburtstagService;
    }

    @GetMapping("/website")
    public String mainpage(Model model) {
        return "index";
    }

    @PostMapping("/newGeburtstag")
    public String newGeburtstag(@RequestParam("nameInput") String name, @RequestParam("dateInput") String dateString){
        System.out.println("Geburtstagskind: " + name);
        System.out.println("Datum: " + dateString);

        String[] arr = dateString.split("-");

        try {
            Geburtstag geb = new Geburtstag(null, name, arr[2], arr[1], arr[0]);
            GeburtstagDTO geburtstagDTO = new GeburtstagDTO(geb);

            geburtstagService.save(geburtstagDTO);
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }

        return "index";
    }
}
