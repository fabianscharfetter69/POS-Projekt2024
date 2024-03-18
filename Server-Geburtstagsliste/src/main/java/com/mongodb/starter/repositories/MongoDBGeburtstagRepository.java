package com.mongodb.starter.repositories;

import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.ClientSession;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.FindOneAndReplaceOptions;
import com.mongodb.client.model.ReplaceOneModel;
import com.mongodb.starter.models.Geburtstag;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;

import static com.mongodb.client.model.Filters.eq;
import static com.mongodb.client.model.Filters.in;
import static com.mongodb.client.model.ReturnDocument.AFTER;

@Repository
public class MongoDBGeburtstagRepository implements GeburtstagRepository {

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

    void init() {
        geburtstagCollection = client.getDatabase("test").getCollection("geburtstage", Geburtstag.class);
    }

    @Override
    public Geburtstag save(Geburtstag geburtstag) {
        geburtstagCollection.insertOne(geburtstag);
        return geburtstag;
    }

    @Override
    public List<Geburtstag> saveAll(List<Geburtstag> geburtstage) {
        try (ClientSession clientSession = client.startSession()) {
            return clientSession.withTransaction(() -> {
                geburtstagCollection.insertMany(clientSession, geburtstage);
                return geburtstage;
            }, txnOptions);
        }
    }
}
