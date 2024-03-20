package com.mongodb.starter;

import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import jakarta.annotation.PostConstruct;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import static com.mongodb.client.model.Filters.eq;

@Repository
public class MongoDBGeburtstagRepository implements GeburtstagsRepository {

    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<Geburtstag> geburtstagCollection;

    public MongoDBGeburtstagRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        geburtstagCollection = client.getDatabase("test").getCollection("geburtstage", Geburtstag.class);
    }

    @Override
    public Geburtstag save(Geburtstag geburtstagEntity) {
        geburtstagEntity.setId(new ObjectId());
        geburtstagCollection.insertOne(geburtstagEntity);
        return geburtstagEntity;
    }

    @Override
    public List<Geburtstag> findAll() {
        return geburtstagCollection.find().into(new ArrayList<>());
    }

    @Override
    public long delete(String id) {
        return geburtstagCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }

    @Override
    public void saveAll(List<GeburtstagDTO> geburtstagDTOs) {
        List<Geburtstag> geburtstagEntities = geburtstagDTOs.stream()
                .map(GeburtstagDTO::toGeburtstagEntity)
                .collect(Collectors.toList());
        geburtstagCollection.insertMany(geburtstagEntities);
    }
}
