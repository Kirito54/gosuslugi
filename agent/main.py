from fastapi import FastAPI
from pydantic import BaseModel
import os
import glob
import numpy as np
from sklearn.feature_extraction.text import TfidfVectorizer

app = FastAPI(title="Gov Agent")

# Load documents from data directory and prepare vectorizer
base_dir = os.path.join(os.path.dirname(__file__), os.pardir, "data")
doc_paths = [p for p in glob.glob(os.path.join(base_dir, "**", "*.md"), recursive=True)]
docs = {p: open(p, encoding="utf-8").read() for p in doc_paths}

vectorizer = None
doc_vectors = None
if docs:
    vectorizer = TfidfVectorizer().fit(list(docs.values()))
    doc_vectors = vectorizer.transform(docs.values())

class AgentRequest(BaseModel):
    text: str

class AgentResponse(BaseModel):
    result: str

@app.post("/analyze", response_model=AgentResponse)
def analyze(req: AgentRequest):
    if vectorizer is None or doc_vectors is None:
        return AgentResponse(result="Нет данных для анализа.")

    query_vec = vectorizer.transform([req.text])
    sims = (doc_vectors @ query_vec.T).toarray().ravel()
    best_idx = int(np.argmax(sims))
    doc_path = doc_paths[best_idx]
    snippet = docs[doc_path][:200].replace("\n", " ")
    return AgentResponse(result=f"Лучшее совпадение: {os.path.basename(doc_path)} - {snippet}")

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
