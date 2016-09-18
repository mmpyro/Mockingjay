FROM mono:latest
WORKDIR /home
ENV EXE MockingJayServer.exe
EXPOSE 51111
COPY ./ /home
CMD mono ./$EXE -h