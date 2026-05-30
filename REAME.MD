# Trabalho Prático - Simuladores de Modelos Computacionais

## Equipe
* **Nome:** Arthur Candido Teixeira | **Matrícula:** 72400072
* **Nome:** Lucas Henrique Albergaria | **Matrícula:** 72401400
* **Nome:** Mateus Soier Ximenes Melo | **Matrícula:** 72401494

---

## Descrição dos Projetos

Este repositório contém as implementações de diferentes modelos computacionais desenvolvidos para a disciplina de Fundamentos Teóricos da Computação. O projeto acompanha a evolução da Hierarquia de Chomsky e é dividido nas seguintes etapas:

### Parte 1: Autômato Finito Determinista (AFD)
Simulador de um AFD para o reconhecimento de Linguagens Regulares. 
* **Características:** O programa permite executar um AFD previamente codificado na classe ou carregar a 5-tupla formal de maneira dinâmica por meio do arquivo `afd.json`. 
* **Entradas:** O simulador lê as cadeias de teste presentes no arquivo `entradasAFD.txt` e valida se são aceitas ou rejeitadas.

### Parte 2: Autômato de Pilha (AP)
Simulador de Autômato de Pilha utilizando critério de aceitação **exclusivo por pilha vazia** (sem verificação de estado final).
* **Linguagem L2 ($a^n b^n \mid n \geq 1$):** Implementação de um AP determinístico para contagem balanceada de símbolos.
* **Linguagem L3 (Palíndromos):** Implementação de um AP não-determinístico, utilizando movimentos-lambda (representados por `\0`) para explorar ramos de execução e "adivinhar" o meio da cadeia.
* **Saída:** O programa exibe a configuração instantânea a cada transição processada (mostrando o estado atual, o conteúdo da pilha e o restante da cadeia lida). As palavras são lidas do arquivo `entradasPILHA.txt`.

### Parte 3: Máquina de Turing (MT)
>[ futuramente com a breve descrição do que foi implementado na simulação da Máquina de Turing, as linguagens tratadas e as lógicas de fita aplicadas.]

---

## Instruções de Compilação e Execução

**Pré-requisitos de Ambiente:**
Os projetos foram desenvolvidos em C# e exigem o **SDK do .NET 6.0** (ou versão superior) instalado na máquina.

###

### Parte 4: Link Video de Defesa
