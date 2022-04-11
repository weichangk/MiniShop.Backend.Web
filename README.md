# MiniShop.Backend.Web


## 使用 dockerfile 构建部署

```shell
docker build -t minishopbackendweb -f Dockerfile .

docker run -d -p 15301:80 --restart=always -v D:/dockervolumes/minishopbackendweb/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopbackendweb/log:/app/log --name minishopbackendweb minishopbackendweb
```





