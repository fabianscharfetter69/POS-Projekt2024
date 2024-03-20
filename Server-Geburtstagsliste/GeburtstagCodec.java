import org.bson.BsonReader;
import org.bson.BsonType;
import org.bson.BsonWriter;
import org.bson.codecs.Codec;
import org.bson.codecs.DecoderContext;
import org.bson.codecs.EncoderContext;

public class GeburtstagCodec implements Codec<Geburtstag> {

    @Override
    public Geburtstag decode(BsonReader reader, DecoderContext decoderContext) {
        reader.readStartDocument();
        String name = reader.readString("name");
        int day = reader.readInt32("day");
        int month = reader.readInt32("month");
        // Hier die Dekodierlogik für Ihr Geburtstagsobjekt implementieren
        reader.readEndDocument();
        return new Geburtstag(name, day, month);
    }

    @Override
    public void encode(BsonWriter writer, Geburtstag value, EncoderContext encoderContext) {
        writer.writeStartDocument();
        writer.writeString("name", value.getName());
        writer.writeInt32("day", value.getDay());
        writer.writeInt32("month", value.getMonth());
        // Hier die Kodierungslogik für Ihr Geburtstagsobjekt implementieren
        writer.writeEndDocument();
    }

    @Override
    public Class<Geburtstag> getEncoderClass() {
        return Geburtstag.class;
    }
}
