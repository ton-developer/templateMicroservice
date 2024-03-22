using Confluent.Kafka;

namespace Infrastructure.Driven.Jobs;

public static class KafkaProducerConfiguration
{
    public static ProducerConfig GetConfig()
    {
        return new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = "KafkaExampleProducer",
        };
    }
}