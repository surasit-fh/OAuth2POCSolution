FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
RUN apt-get update && apt-get install -y procps \
 && apt-get install -y tzdata
RUN apt-get install -y net-tools && apt-get install -y telnet
RUN ln -fs /usr/share/zoneinfo/Asia/Bangkok /etc/localtime
RUN dpkg-reconfigure --frontend noninteractive tzdata
#WORKDIR /usr/src/bin
COPY . ./

ENTRYPOINT ["dotnet", "OAuth2POC.IDP.dll"]
