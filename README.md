# Demo.Bank.Accounts

## TODO:

- [ ] Docker support with docker compose. 
- [ ] Using SQL server as the database.
- [ ] Database migration as a sidecar.
- [ ] Using storage queues for events.
- [ ] Background service which listens to the queue, and perform operations.

## Features

### Create bank account

```mermaid
sequenceDiagram
    client->>API: create bank account request
    API->>API: validate request
    alt is invalid?
        API ->>client: error response
    end
```

### Transfer money between accounts
