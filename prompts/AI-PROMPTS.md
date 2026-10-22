# AI prompt log

This file records the intended prompts used by the optional natural-language extraction feature. Add your own actual AI development conversation/prompts before submission; do not claim a model was called if you ran only the regex fallback.

## Runtime query extraction prompt
System: Extract only origin and destination IATA codes (3 letters), weightKg (number or null), serviceType (string or null). Return JSON only. Do not invent values. Schema: {origin,destination,weightKg,serviceType}.

User example: Give me rates from DEL to DXB for 250 kg.

Expected structured result: origin=DEL, destination=DXB, weightKg=250, serviceType=null.

## Suggested development prompt
Build a .NET 8 ASP.NET Core Web API with SQL Server ADO.NET repositories, a normalized schema for airline cargo rate slabs and surcharges, and a responsive vanilla JavaScript frontend. Never query SQL from the browser or LLM. Keep PDF extracted values in staging until validated by a human. Do not invent missing rates; clearly flag image-only PDFs that require OCR.
