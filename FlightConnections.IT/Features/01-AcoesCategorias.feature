#language: pt-br
Funcionalidade:
Como administrador de cadastro de rota de avião
Eu quero cadastrar, editar, excluir e consultar rotas e vaalores
Para que os usuários do sistema possam associá-los ao sua viagem e escolher a rota que melhor atende

#Regras: As Rotas deve possuir um código de identificação, uma origem, um  destino e um valor
Cenário: 0100 - Cadastrar rotas
	Dados as seguintes informações para cadastro de rotas:
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   10  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |
		| 4  |        |         |       |
	Quando cadastrar as rotas
	Entao devem existir as rotas cadastradas
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   10  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |

Cenário: 0101 - Editar rotas
	Dados as seguintes informações para edição por id:
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   10  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |
	Quando editar a rota por id
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |
	Entao devem existir todas as rotas e a editada
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |

Cenário: 0102 - Excluir rotas
	Dados as seguintes informações para exclusao por id:
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   10  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |
	Quando excluir a rota por id
		| Id | Origin | Destiny | Value |
		| 3  |  GRU   |   CDG   |   10  |
	Entao devem existir apenas os seguintes retornos
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   10  |
		| 2  |  GRU   |   ORL   |   10  |

Cenário: 0103 - Consultar todas as rotas
	Dados as seguintes informações para a consulta de todas as rotas:
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |
	Quando consultar todas as rotas cadastradas
	Entao devem existir as rotas consultadas
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |

Cenário: 0104 - Consultar rota por id
	Dados as seguintes informações para a consulta de rota por id:
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |
		| 2  |  GRU   |   ORL   |   10  |
		| 3  |  GRU   |   CDG   |   10  |
	Quando eu consultar rota cadastrada por id
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |
	Entao deve existir a rota consultada por id
		| Id | Origin | Destiny | Value |
		| 1  |  GRU   |   BRC   |   20  |