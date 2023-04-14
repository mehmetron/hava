

```bash
# Run init bash
cd hava/Dockerfiles/postgres

# Delete old containers and images
docker stop my-postgres-container
docker rm my-postgres-container
docker rmi my-postgres
docker volume rm pgdata

# Create new volume and run container
docker build -t my-postgres .
docker volume create pgdata
docker run -p 5432:5432 --name my-postgres-container -v pgdata:/var/lib/postgresql/data -d my-postgres
docker ps

# Delete Migrations directory and create new migrations
cd ../..
rm -rf Migrations
dotnet ef migrations add initial
dotnet ef database update

# Run Tests which also seeds DB because of Auth integration test
cd ../Testing
dotnet test
```





# See Everything
```bash
docker ps
systemctl status nginx
```

# On Database Updates
```bash
cd hava
dotnet ef migrations add initial
dotnet ef database update
```

# For production
```bash
sudo systemctl start postgresql
sudo systemctl start apichipper.service
sudo systemctl start nginx
```

### For creating new service for .net core app in production:
#### https://www.linode.com/docs/guides/start-service-at-boot/
 

# For development

### nginx
```bash
sudo systemctl start nginx
```

### postgresql
#### run docker container

### apichipper
#### run app in terminal# hava
