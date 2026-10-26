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
                script {
                    if (isUnix()) {
                        sh 'ls -la'
                    } else {
                        bat 'dir'
                    }
                }
            }
        }

        stage('Setup') {
            steps {
                echo 'Восстановление NuGet-зависимостей...'
                script {
                    if (isUnix()) {
                        sh 'dotnet restore Lab3.sln'
                    } else {
                        bat 'dotnet restore Lab3.sln'
                    }
                }
            }
        }

        stage('Build') {
            steps {
                echo 'Сборка проекта...'
                script {
                    if (isUnix()) {
                        sh 'dotnet --info'
                        sh 'dotnet build Lab3.sln -c Release --no-restore'
                    } else {
                        bat 'dotnet --info'
                        bat 'dotnet build Lab3.sln -c Release --no-restore'
                    }
                }
            }
        }

        stage('Test') {
            steps {
                echo 'Запуск тестов...'
                script {
                    if (isUnix()) {
                        sh 'dotnet test Lab3.sln -c Release --no-build --verbosity normal'
                    } else {
                        bat 'dotnet test Lab3.sln -c Release --no-build --verbosity normal'
                    }
                }
            }
        }

        stage('Lint') {
            steps {
                echo 'Статический анализ (analyzers / build warnings)...'
                script {
                    if (isUnix()) {
                        sh 'dotnet build 3/3.csproj -c Release -warnaserror:0 > analysis_ci.txt || true'
                    } else {
                        bat 'dotnet build 3\\3.csproj -c Release > analysis_ci.txt 2>&1 || exit /b 0'
                    }
                }
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
                script {
                    if (isUnix()) {
                        sh '''
                          mkdir -p artifacts
                          echo "Build ${BUILD_NUMBER}" > artifacts/build.txt
                          echo "Project ${PROJECT_NAME}" >> artifacts/build.txt
                        '''
                    } else {
                        bat """
                          if not exist artifacts mkdir artifacts
                          echo Build %BUILD_NUMBER%> artifacts\\build.txt
                          echo Project %PROJECT_NAME%>> artifacts\\build.txt
                        """
                    }
                }
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
