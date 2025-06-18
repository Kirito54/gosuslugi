from fastapi import FastAPI
from pydantic import BaseModel
import os

app = FastAPI(title="Gov Agent")

class AgentRequest(BaseModel):
    text: str

class AgentResponse(BaseModel):
    result: str

@app.post("/analyze", response_model=AgentResponse)
def analyze(req: AgentRequest):
    # TODO: integrate LLM and vector DB
    return AgentResponse(result="Заявление обработано (заглушка).")

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
