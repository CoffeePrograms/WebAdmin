# WebAdmin

Запускается в докере. Студия может ругаться на собственноручно созданный файл, поэтому вернее запускать через консоль 
docker-compose up -d

Запускается по https.
WebMain   https://localhost:8081/swagger/index.html
WebAdmin  https://localhost:8085/swagger/index.html

RabbitMQ — для передачи логов.

ClickHouse — для хранения логов.

Prometheus — для мониторинга метрик.

Grafana — для визуализации данных и метрик.
