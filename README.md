# Um melhor validador de senhas 

Um validador de senhas que usa Regex para validar quaisquer senhas. Isso reduz bastante código.

---

# Critérios de validação

- Pelo menos nove caracteres
- Pelo menos um dígito
- Pelo menos uma letra minúscula
- Pelo menos uma letra maiúscula
- Pelo menos um caractere especial
- Os seguintes caracteres são considerados especiais: !@#$%^&*()-+
- Não pode haver caracteres redundantes dentro do conjunto
- Espaços em branco não são considerados caracteres

---

# Exemplos de validação

```c#
IsValid("") // false  
IsValid("aa") // false  
IsValid("ab") // false  
IsValid("AAAbbbCc") // false  
IsValid("AbTp9!foo") // false  
IsValid("AbTp9!foA") // false
IsValid("AbTp9 fok") // false
IsValid("AbTp9!fok") // true
```

---

# Como executar

Abrir a solução no Visual Studio e executar o projeto ao clicar no botão de executar deveria ser o suficiente para compilar e executar o projeto. <br>
Além disso, é esperado que o Swagger seja aberto no seu navegador principal. <br>
Com o Swagger aberto, clique na única rota "api/Password" e então clique no botão "Try it out". <br>
Depois disso, digite qualquer senha no campo textbox e clique no botão "Execute". <br>

---

# Sobre a solução

Há diversas maneiras de validar uma senha. No instante em que eu havia lido o arquivo readMe, a primeira coisa que veio em minha mente foi usar Regex. <br>
Ao usar Regex, sou capaz de especificar padrões para validar senhas em uma única linha de código. Essa é uma maneira rápida e limpa de validar senhas. <br>
A validação de senha é baseada em quatro regras condicionais: <br>

1- Verifica se a senha possui caracteres o suficiente ou não <br>

```c#
if (senha.Length < 9) 
  return BadRequest(false);
```

2- Verifica se a senha contém pelo menos uma letra maiúscula, uma letra minúscula, um dígito e um caractere especial definido por um conjunto de caracteres especiais. <br>
Em outras palavras, é verificado se a senha condiz com o padrão fornecido ou não. <br>
2.1- O conjunto definido de caracteres especiais é: !@#$%^&*()-+ <br>

```c#
else if (!Regex.IsMatch(senha, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()\-+]).+$"))
   return BadRequest(false);
```

3- Verifica se há caracteres repetidos na string de senha ou não. <br>

```c#
else if (Regex.IsMatch(senha, @"(.).*\1"))
    return BadRequest(false);
```

4- A senha é válida. <br>

```c#
else
    return Ok(true);
```

---

# Tratamento de erros

Quando o cliente envia uma senha inválida, isto é, quando a senha não passa em uma das regras condicionais anteriormente explicadas, isso é considerado um erro do cliente. <br>
O status code 4xx do HTTP é dedicado a erros do cliente. O status code 400 é para requisições ruins (bad requests), isto é, quando o cliente envia uma requisição HTTP contendo o conteúdo errado, como uma senha inválida. <br>
Para cada momento que a senha enviada pelo cliente não passa uma regra condicional (ou regra de validação), o servidor envia uma resposta HTTP possuindo o status code 400, enviando o valor "false" em seu corpo. <br>

Ademais, também há a possibilidade de quando o usuário testar a aplicação através do Swagger, ele envia um valor nulo como uma senha. <br>
Para evitar esse problema, a exceção NullReferenceException é tratada: <br>


```c#
catch (NullReferenceException nrex)
{
   return BadRequest(false);
}
```

Mesmo assim, a aplicação ainda anuncia que a senha é inválida. <br>

Para tratar todos os casos possíveis de exceção, garantindo que a aplicação jamais poderá ser fechada abruptamente, um tratamento de exceção genérico é escrito, assumindo que esse seja um erro do servidor: <br>

```c#
catch (Exception ex)
{
   return StatusCode(500);
}
```

É importante enfatizar que, nesse caso, a exceção deveria ser salva dentro de um arquivo de log, e desde que esse erro poderia ser qualquer coisa (e desconhecido), não é trivial definir uma mensagem específica para o usuário. <br>
A aplicação front-end deveria disparar uma mensagem genérica ao usuário ao ler um status code 500 dentro de uma resposta HTTP.

---


# Better Password Validator 
A password validator which uses straightforward Regex to validate any passwords. It reduces code quite a lot.

---

# Validation criteria

- At least nine characters
- At least one number
- At least one lower case letter
- At least one upper case letter
- At least one special character
- The following characters must be considered as special: !@#$%^&*()-+
- There must not be any redundant characters within the set
- White or blank spaces must not be considered valid characters

---

# Validation examples

```c#
IsValid("") // false  
IsValid("aa") // false  
IsValid("ab") // false  
IsValid("AAAbbbCc") // false  
IsValid("AbTp9!foo") // false  
IsValid("AbTp9!foA") // false
IsValid("AbTp9 fok") // false
IsValid("AbTp9!fok") // true
```

---

# How to execute

Opening the solution on Visual Studio and running the project by clicking on the "run" button should be enough to build and run the project. <br>
Moreover, it is expected that Swagger opens on your main browser. <br>
With Swagger open, click on the single route "api/Password" and then click on "Try it out". <br>
Afterwards, type any password on the textbox field and click on "Execute" button. <br>

---

# About the solution

There are several ways to validate a password. By the time I had read the problem on readME file, the first thing that had crossed my mind was using Regex. <br>
By using Regex, I am capable of specifying patterns to validate password on a single code line. It is a fast and clean way to validate passwords. <br>
The password validation is based on four conditional rules: <br>

1- Checks whether the password has enough characters or not <br>

```c#
if (senha.Length < 9) 
  return BadRequest(false);
```

2- Checks if the password contains at least one uppercase letter, one lowercase letter, one digit and one special character defined by a set of special characters. <br>
In other words, it checkes whether the password matches the provided pattern or not. <br>
2.1- The defined set of special characters is: !@#$%^&*()-+ <br>

```c#
else if (!Regex.IsMatch(senha, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()\-+]).+$"))
   return BadRequest(false);
```

3- Checks whether there are repeated characters within the password string or not: <br>

```c#
else if (Regex.IsMatch(senha, @"(.).*\1"))
    return BadRequest(false);
```

4- The password is valid. <br>

```c#
else
    return Ok(true);
```

---

# Error handling

When the client sends an invalid password, i.e., when the password doesn't pass one of the conditional rules previously explained, it is considered a client error. <br>
The HTTP 4xx status code is dedicated to client errors. The 400 status code is for bad requests, i.e., when the client sends an HTTP request containing wrong content, like an invalid password. <br>
For each time the password sent by the client does not pass a conditional rule (or validation rule), the server sends an HTTP response possessing 400 status code, sending the "false" value in its body. <br>

Furthermore, it is also possible that when the user tests the application through swagger, they send a null value for password. <br>
To avoid that problem, the NullReferenceException is caught and handled: <br>

```c#
catch (NullReferenceException nrex)
{
   return BadRequest(false);
}
```

Even so, the application still announces that the password is invalid. <br>

To catch all possible exception cases, making sure that the application might never shut down, a generic exception handling is placed, assuming that it is a server error: <br>

```c#
catch (Exception ex)
{
   return StatusCode(500);
}
```

It is important to point out that, in this case, the exception should be saved into a log file, and since this error could be anything (and unknown), it is not trivial to define a specific message to the user. <br>
The front-end application should raise a generic message to the user when reading a 500 HTTP status code in an HTTP response.


