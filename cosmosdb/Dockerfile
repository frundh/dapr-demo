FROM mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator
RUN apt-get update && apt-get install -y --no-install-recommends curl ca-certificates && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

COPY entrypoint.sh .
COPY setup.sh .
RUN chmod +x entrypoint.sh setup.sh

HEALTHCHECK --interval=15s --timeout=5s --start-period=60s --retries=10 CMD curl -IXGET --fail https://localhost:8081/_explorer/index.html || exit 1
CMD ["./entrypoint.sh"]