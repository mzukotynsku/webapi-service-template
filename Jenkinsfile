import groovy.json.JsonOutput

def CURRENT_VERSION = ""

node {
    def gitInfo

    def commonParameters = [
        projectName: 'template'
    ]

    stage('Build & Test') {
        echo 'Building and testing'
        gitInfo = checkout scm
        setVersionNumber()

        runTests()
        publishTestResults()

        // buildAndPushImages(commonParameters)
    }

    stage('Publish packages') {
        echo 'Publishing packages'
        // publishPackages()
    }
}

def setVersionNumber() {
	def majorMinor = "1.0"
	if (fileExists('version.txt')) {
		majorMinor = readFile('version.txt').trim()
	}

	CURRENT_VERSION = "${majorMinor}.${BUILD_NUMBER}"
}

def runTests() {
    sh "docker-compose -f docker-compose.testrunner.yml up --force-recreate --abort-on-container-exit"
    sh "docker-compose -f docker-compose.testrunner.yml down --rmi local -v --remove-orphans"
}

def publishTestResults() {
    mstest testResultsFile:"./testresults/*.trx", keepLongStdio: true
}

def buildAndPushImages(parameters) {
    def registry = "my-registry:50001"
    echo "Building ${parameters.projectName}"

    def TemplateCoreApi = docker.build("${registry}/template-core-api:${CURRENT_VERSION}", "-f ./src/Template.WebApi/Dockerfile .")
    def TemplateCoreEndpoint = docker.build("${registry}/template-core-endpoint:${CURRENT_VERSION}", "-f ./src/Template.MessageProcessor/Dockerfile .")

    echo "Push image to registry"
    TemplateCoreApi.push()
    TemplateCoreEndpoint.push()
}

def publishPackages() {
    def registry = "my-registry:50001"
    def imageName = "${registry}/template-package-publisher:${CURRENT_VERSION}"
    def packagePublisher = docker.build(imageName, "--build-arg Version=${CURRENT_VERSION} -f ./Dockerfile.Publisher .")
	try {
		packagePublisher.run("--rm -v c:/Dev/packages:/packages", "--source /packages")
	}
	finally {
		echo "Removing publisher image"
		docker.node {
			docker.script.sh "docker rmi ${imageName} -f"
		}
	}
}