# P2P Auction RPC

## Getting Started

### Explanation

The solution is a gRPC solution backed iin a DHT (see Setting up the DHT) using bitfinex grenache

```bash
git clone git@github.com:paulkagiri/bfx-challenge.git
```

### Install Project Dependencies and run grenache servers

Before running the solkutions you have to install DHT grenache-grape network dependencies 

First, install the Grape server globally:

```bash
npm install -g grenache-grape
```
Then, in separate terminal windows start two Grape servers by running:

```bash
grape --dp 20001 --aph 30001 --bn '127.0.0.1:20002'
grape --dp 20002 --aph 40001 --bn '127.0.0.1:20001'
```


### Start the Clients

First you need to build the solutions by running at solution level:

```bash
dotnet build
```

To run the clients, open different terminal and run :

```bash
dotnet build
```

You can start multiple clients to simulate interactions with the order book. If you need more detailed output for debugging purposes, use the following command instead:

```bash
npm run client:debug
```

## Implementation

Each node serves both as grpc client and server. The discovery phasew of the network is beng done using Grenache (refer to `Grenache Grape`: https://github.com/bitfinexcom/grenache-grape:)

## Known Issues

The project is functional but it has some missing functionalities

1. **Close use case**: I wasn't been able ot make it because of time. The implementation should close the auction (in the in memory by removing the open auctions hashset).

2. **Database backbone**: I couldn't implement a DB persistance for the auction state, yet I used a repository pattern and DI, so we can create the DB repository and inject it via IOC.

3. **Client Cache**: When a client is aborted, its IP address remains in the DHT cache, which can generate network errors for other clients. Finding a way to correctly and completely disconnect a client from the DHT is a potential improvement. Additionally, a restart of the Grape servers may be required to flush the cache when restarting the client.

4. **Initial sync**: the application must, at intialization, ask a sever for the latest state by getting a connected server and ask for what's it's the open auctions. This would allow for a client to disconnect and reconnect again without loosing the state and at the same time the state is distributed.