pipeline {
    agent any

    environment {
        PROJECT_NAME = 'lab2-vcs'
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {
        stage('Checkout') {
            steps {
                echo "Клонирование проекта ${env.PROJECT_NAME}..."
                checkout scm
                echo "Workspace: ${env.WORKSPACE}"
            }
        }

        stage('Setup') {
            steps {
                echo 'Восстановление NuGet-зависимостей...'
                bat 'dotnet restore Lab3.sln'
            }
        }

        stage('Build') {
            steps {
                echo 'Сборка проекта...'
                bat 'dotnet --version'
                bat 'dotnet build Lab3.sln -c Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                echo 'Запуск тестов...'
                bat 'dotnet test Lab3.sln -c Release --no-build --verbosity normal'
            }
        }

        stage('Lint') {
            steps {
                echo 'Статический анализ...'
                bat 'dotnet build 3\\3.csproj -c Release > analysis_ci.txt 2>&1 || exit /b 0'
            }
            post {
                always {
                    archiveArtifacts artifacts: 'analysis_ci.txt', allowEmptyArchive: true
                }
            }
        }

        stage('Archive') {
            steps {
                echo 'Сохранение артефакта...'
                bat 'if not exist artifacts mkdir artifacts'
                bat 'echo Build %BUILD_NUMBER%> artifacts\\build.txt'
                bat 'echo Project %PROJECT_NAME%>> artifacts\\build.txt'
                archiveArtifacts artifacts: 'artifacts/**', allowEmptyArchive: true
            }
        }
    }

    post {
        always {
            echo "Pipeline завершён. Номер сборки: ${env.BUILD_NUMBER}"
        }
        success {
            echo 'Все этапы прошли успешно!'
        }
        failure {
            echo 'Один из этапов завершился с ошибкой'
        }
    }
}
