package com.mongodb.starter.controller;


import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class WebController {
    @GetMapping("/website")
    public String mainpage(Model model) {
        return "index";
    }

    @GetMapping("/newGeburtstag")
    public String newGeburtstag(@RequestParam("nameInput") String name){
        System.out.println("Geburtstagskind: " +name);
        return "index";
    }

}
