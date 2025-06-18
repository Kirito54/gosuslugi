FROM python:3.11-slim
WORKDIR /app
COPY agent/requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt
COPY agent /app/agent
CMD ["uvicorn", "agent.main:app", "--host", "0.0.0.0", "--port", "8000"]
