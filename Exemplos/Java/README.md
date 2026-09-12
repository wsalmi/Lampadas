# Exemplo Java

Requer JDK 17 e Maven 3.9 ou superior.

```bash
export LAMPADAS_API_URL=http://localhost:3000/api
export COD_EQUIPE=0
mvn test
mvn package
java -cp target/classes com.stefanini.lampada.App
```

Use `--repeat` para repetir a sequência durante uma apresentação. Sem a opção, uma rodada é executada e o processo termina.
