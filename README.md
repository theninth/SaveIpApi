Save ip API
===========

A simple key/value store based on sqlite.

## Example produktion docker-compose.yaml file

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

## Start interacting with database

### Storing

This is an example how you can store your public ip address with some help of
[ipify](https://www.ipify.org/) from a simple shell script.

```sh
#!/bin/sh
MYIP_AS_JSON=$(curl 'https://api.ipify.org?format=json')
curl -H 'Content-Type: application/json'                  \
     -H 'ApiKey: changeme'                                \
     -d "$MYIP_AS_JSON"                                   \
     -X POST                                              \
     https://saveipapi.example.com/ip/some_key
```

Change saveipapi url, apikey and probably the value "some_key" to something more relevant.

Or you can use [PyUpdateSaveIpAPI](https://github.com/theninth/PyUpdateSaveIpApi).

### Retrieving

```sh
curl -H 'ApiKey: changeme' https://saveipapi.example.com/ip/some_key
```