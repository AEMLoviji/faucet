# Faucet API

Faucet is a cryptocurrency reward system which gives out small amounts of coins or tokens to anonymous users.

## Requirements were

- Implement REST api that provides methods:
  - get current faucet bitcoin balance and value (in $);
  - claim 0.001 bitcoin from the faucet, by provided email.
    - If the faucet doesn't have the required amount, don't claim anything.
    - Each email can only claim once per 24h.
    - Claim simply reduces faucet balance by claimed bitcoin amount.
- Initial balance at server overall start is 0. Upon further server restarts balance is persisted.
- Have Scheduled job running every 2 hours, adding $500 worth of bitcoin to the faucet.
  - You'd need to get the live bitcoin price to know how much bitcoin to add. Up to you where to get.
- Scheduled job running every 24 hours, sending out email to admin about total claimed amount since last sent email.
- Store data in embedded DB with ORM on top.
- Code must be async.
- Use dependency injection for components.
- Use logging.
- Generate REST api documentation.
- Write meaningful integration tests for the REST api.

## Before running

Please make sure to change parameters below in `appsettings.json` file.

- `To` - admin's email 
- `SenderPassword` - It will be provided by an email

```json
"EmailOptions": {
	"From": "faucetapp2021@gmail.com",
	"To": "",
	"SenderPassword": ""
}
```
