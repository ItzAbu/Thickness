# -*- coding: utf-8 -*-
from faker import Faker
import json
from datetime import datetime

fake = Faker('it_IT')

def generate_fake_codf(nome, cognome, data_nascita, citta):
    return f"{cognome[:3].upper()}{nome[:3].upper()}{data_nascita.strftime('%y%m%d')}{citta[:2].upper()}X"

users = []
for _ in range(200):
    nome = fake.first_name()
    cognome = fake.last_name()
    data_nascita = fake.date_of_birth(minimum_age=18, maximum_age=90)
    citta = fake.city()
    
    provincia = fake.state_abbr()
    user = {
        "nome": nome,
        "cognome": cognome,
        "email": f"{nome.lower()}.{cognome.lower()}@example.com",
        "CodF": generate_fake_codf(nome, cognome, data_nascita, citta),
        "numeroTelefono": f"+39{fake.msisdn()[3:]}",
        "gender": fake.random_element(elements=("M", "F")),
        "dataNascita": data_nascita.strftime("%Y-%m-%d"),
        "residenza": f"{citta}, {provincia}"
    }
    users.append(user)

with open('utenti.json', 'w', encoding='utf-8') as f:
    json.dump(users, f, indent=2, ensure_ascii=False)  # Rimuovi il wrapper "utenti"