# GovServicesSolution

Проект демонстрирует использование Blazor и ASP.NET для системы электронного делопроизводства.

Проект включает C#‑endpoint для автоматической классификации загружаемых документов.

## Быстрый старт

1. Установите Docker и Docker Compose.
2. Запустите сборку:
   ```bash
   docker-compose up --build
   ```
3. Веб‑клиент будет доступен на [http://localhost:5002](http://localhost:5002).
4. API сервера — на [http://localhost:5001](http://localhost:5001).

### Классификация документов

Сервер предоставляет метод `POST /api/classify` для распознавания типа документа и извлечения основных полей. Можно передать текст напрямую или загрузить файл (PDF, JPEG/PNG и др.). Пример отправки текста:

```bash
curl -X POST http://localhost:5001/api/classify \
     -F text="Паспорт серия 12 34 номер 567890 выдан МВД 01.02.2003"
```

Ответ будет содержать определённый тип и найденные поля:

```json
{
  "type": "passport",
  "fields": {
    "series": "12 34",
    "number": "567890",
    "issued_by": "мвд",
    "issue_date": "01.02.2003"
  }
}
```

Для загрузки файла используйте `multipart/form-data`:

```bash
curl -X POST http://localhost:5001/api/classify \
     -F file=@passport.pdf
```
