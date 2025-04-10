# Cours Entity Framework

## Lancer le TP EventManagerAPI-TP

### 1. Cloner le projet

Pour lancer la partie backend du TP **EventManagerAPI-TP**, commencez par cloner le repo via la commande suivante :

```bash
git clone git@github.com:RemiLecas/EntityFrameWork.git
```

### 2. Construire et exécuter le projet
Une fois le dépôt cloné, ouvrez le dossier EventManagerAPI-TP et exécutez les commandes suivantes pour construire et démarrer l'application :

```bash
dotnet build
dotnet run
```
Important : Avant de lancer dotnet run, assurez-vous que **MySQL** soit démarré en local.


### 3. Configuration de la connexion à la base de données
Si la connexion à la base de données échoue, modifiez le fichier appsettings.json pour ajuster les informations de connexion à la base de données MySQL. Mettez à jour la section suivante avec les informations appropriées :

```json
"ConnectionStrings": {
    "EventManagerBDD": "server=ip-de-la-base;database=EventManagerAPITP;user=nom-du-user;password=mot-de-passe-du-user;"
  }
}
```

### 4. Accéder à Swagger
Une fois la connexion établie, vous pouvez accéder à l'interface Swagger pour tester les routes de l'API en vous rendant à l'URL suivante :

http://localhost:5257/swagger/index.html

### 5. Utiliser l'interface front-end
Si vous préférez une interface web pour manipuler les données des événements, allez dans le dossier event-manager-front, puis exécutez les commandes suivantes pour installer les dépendances et démarrer l'application front-end :

```bash
npm install
npm run start
```

### 6. Lancer les tests
Pour exécuter les tests, allez dans le dossier EventManagerAPI-TP.Tests qui se trouve dans EventManagerAPI-TP, puis exécutez la commande suivante :

```bash
dotnet test
```


### 7. Problèmes potentiels
Attention : Il peut y avoir des problèmes lorsque les tests ne se lancent pas ou lorsque l'API ne peut plus démarrer après l'exécution des tests.

Si cela se produit, supprimez les fichiers créés dans EventManagerAPI-TP.Tests et relancez l'API.