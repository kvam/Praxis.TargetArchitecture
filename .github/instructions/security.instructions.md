---
description: Secrets handling and safe output rules.
name: Security
applyTo: "**"
---

# Security

- Never write secrets, tokens, passwords, or connection strings into tracked files.
- Use user-secrets for local backend secrets.
- Validate user input before using it in file paths, HTTP calls, or database queries.
