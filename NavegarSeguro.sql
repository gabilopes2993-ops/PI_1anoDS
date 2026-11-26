CREATE DATABASE IF NOT EXISTS navegarseguro;

USE navegarseguro;

CREATE TABLE IF NOT EXISTS Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Senha VARCHAR(100) NOT NULL UNIQUE,
    TipoUsuario VARCHAR(20) NOT NULL DEFAULT 'Jogador'

 
);
 
INSERT INTO Usuarios (Nome, Email, Senha, TipoUsuario)
VALUES ('Administrador', 'admin@gmail.com', 'admin123', 'Admin');


 
CREATE TABLE IF NOT EXISTS Perguntas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Enunciado TEXT NOT NULL,
    AlternativaA VARCHAR(255) NOT NULL,
    AlternativaB VARCHAR(255) NOT NULL,
    AlternativaC VARCHAR(255) NOT NULL,
    AlternativaD VARCHAR(255) NOT NULL,
    RespostaCorreta CHAR(1) NOT NULL,
    Explicacao TEXT,
    Pontos INT NOT NULL
);
 
INSERT INTO Perguntas
(Enunciado, AlternativaA, AlternativaB, AlternativaC, AlternativaD, RespostaCorreta, Explicacao, Pontos)
VALUES
 
(
'Qual é a principal função de uma senha forte?',
'Facilitar o acesso',
'Proteger contas e informações',
'Compartilhar dados',
'Deixar o computador mais rápido',
'B',
'Uma senha forte dificulta o acesso de pessoas não autorizadas.',
10
),
 
(
'O que é phishing?',
'Um tipo de computador',
'Um jogo online',
'Uma tentativa de roubar informações por meio de golpes',
'Um programa de edição',
'C',
'Phishing é um golpe que tenta obter dados pessoais por mensagens ou páginas falsas.',
10
),
 
(
'O que devemos fazer ao receber um link suspeito?',
'Clicar imediatamente',
'Compartilhar com amigos',
'Verificar a origem antes de acessar',
'Informar nossa senha',
'C',
'É importante verificar a origem dos links antes de clicar.',
10
),
 
(
'Qual informação não devemos compartilhar publicamente?',
'Uma música favorita',
'Uma senha pessoal',
'Um filme favorito',
'Uma opinião sobre jogos',
'B',
'Senhas são informações pessoais e devem permanecer protegidas.',
10
),
 
(
'Para que serve um antivírus?',
'Proteger o dispositivo contra programas maliciosos',
'Aumentar o volume',
'Criar documentos',
'Melhorar a internet',
'A',
'Um antivírus ajuda a identificar e bloquear ameaças digitais.',
10
),
 
(
'O que é uma rede Wi-Fi pública?',
'Uma rede exclusiva para empresas',
'Uma rede disponível em locais públicos',
'Uma rede sem internet',
'Uma rede protegida por senha pessoal obrigatoriamente',
'B',
'Redes públicas podem ser encontradas em locais como aeroportos e restaurantes.',
10
),
 
(
'O que devemos fazer ao receber uma mensagem de um desconhecido pedindo dinheiro?',
'Enviar imediatamente',
'Compartilhar dados bancários',
'Verificar a identidade da pessoa',
'Enviar nossa senha',
'C',
'É importante confirmar a identidade da pessoa antes de realizar qualquer transferência.',
10
),
 
(
'Qual é a importância da atualização dos aplicativos?',
'Deixar a tela mais bonita',
'Corrigir falhas e melhorar a segurança',
'Apagar todas as contas',
'Compartilhar informações',
'B',
'Atualizações podem corrigir vulnerabilidades e melhorar a proteção.',
10
),
 
(
'O que é autenticação em dois fatores?',
'Usar duas contas diferentes',
'Ter duas senhas iguais',
'Uma camada extra de proteção no login',
'Compartilhar a senha com alguém',
'C',
'A autenticação em dois fatores adiciona uma segunda etapa de verificação.',
10
),
 
(
'O que devemos fazer ao perceber que uma conta foi invadida?',
'Ignorar o problema',
'Compartilhar a senha',
'Alterar a senha e buscar recuperar a conta',
'Excluir todos os aplicativos',
'C',
'É importante proteger a conta, alterar a senha e utilizar os recursos oficiais de recuperação.',
10
);
 
 
 
CREATE TABLE IF NOT EXISTS Ranking (
    IdRanking INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Pontuacao INT NOT NULL,
 
    CONSTRAINT FK_Ranking_Usuarios
    FOREIGN KEY (UsuarioId)
    REFERENCES Usuarios(Id)
    );
    
    
    
CREATE TABLE IF NOT EXISTS RankingPerguntas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdRanking INT NOT NULL,
    PerguntaId INT NOT NULL,

    CONSTRAINT FK_RankingPerguntas_Ranking
    FOREIGN KEY (IdRanking)
    REFERENCES Ranking(IdRanking),

    CONSTRAINT FK_RankingPerguntas_Perguntas
    FOREIGN KEY (PerguntaId)
    REFERENCES Perguntas(Id)
);