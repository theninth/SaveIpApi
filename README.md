Save ip API
===========

A simple key/value store based on sqlite.

## Example produktion docker-compose.yaml -file

```yaml
services:
  saveipapi:
    image: thenajnth/saveipapi:22  # Change version number
    container_name: saveipapi
    ports:
      - "8080:8080"
    env_file:
      - .env
    volumes:
      - saveipapi-data:/data
  sqliteweb:
    image: tomdesinto/sqliteweb
    ports:
      - 8081:8080
    volumes:
      - saveipapi-data:/data
    command: /data/ipaddresses.db
  seq:
    image: datalust/seq:latest
    ports:
      - "5341:80"
    environment:
      ACCEPT_EULA: "Y"
    volumes:
      - seq-data:/data
    restart: unless-stopped
volumes:
  saveipapi-data:
  seq-data:
```

This file should be accompanied with a .env-file to override appsettings.json. For example:

```
AUTHENTICATION__APIKEY=MySuperSecretApiKey
```

### Containers

- **saveipapi** - Main application
- **sqliteweb** - DB Web UI
- **seq** - Logs WebUI