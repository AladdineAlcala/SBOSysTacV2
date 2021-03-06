var log4javascript;
(function() {
	function ff() {
		return function() {};
	}
	function copy(obj, props) {
		for (var i in props) {
			obj[i] = props[i];
		}
	}
	var f = ff();

	// Loggers
	var Logger = ff();
	copy(Logger.prototype, {
		addAppender: f,
		removeAppender: f,
		removeAllAppenders: f,
		log: f,
		setLevel: f,
		getLevel: f,
		trace: f,
		debug: f,
		info: f,
		warn: f,
		error: f,
		fatal: f
	});

	var getLogger = function() {
		return new Logger();
	};

	log4javascript = {
		isStub: true,
		version: "dummy",
		logLog: {
			setQuietMode: f,
			setAlertAllErrors: f,
			debug: f,
			warn: f,
			error: f
		},
		addErrorListener: f,
		removeErrorListener: f,
		setEnabled: f,
		setShowStackTraces: f,
		isEnabled: f,
		evalInScope: f,
		getLogger: getLogger,
		getDefaultLogger: getLogger,
		getNullLogger: getLogger,
		Level: ff(),
		LoggingEvent: ff(),
		Layout: ff(),
		Appender: ff()
	};

	// LoggingEvents
	log4javascript.LoggingEvent.prototype = {
		getThrowableStrRep: f
	};

	// Levels
	log4javascript.Level.prototype = {
		toString: f,
		equals: f,
		isGreaterOrEqual: f
	};
	var level = new log4javascript.Level();
	copy(log4javascript.Level, {
		ALL: level,
		TRACE: level,
		DEBUG: level,
		INFO: level,
		WARN: level,
		ERROR: level,
		FATAL: level,
		OFF: level
	});

	// Layouts
	log4javascript.Layout.prototype = {
		defaults: {},
		format: f,
		ignoresThrowable: f,
		getContentType: f,
		allowBatching: f,
		getDataValues: f,
		setKeys: f,
		setCustomField: f,
		hasCustomFields: f
	};

	// SimpleLayout
	log4javascript.SimpleLayout = ff();
	log4javascript.SimpleLayout.prototype = new log4javascript.Layout();

	// XmlLayout
	log4javascript.XmlLayout = ff();
	log4javascript.XmlLayout.prototype = new log4javascript.Layout();
	log4javascript.XmlLayout.prototype.escapeCdata = f;

	// JsonLayout
	log4javascript.JsonLayout = ff();
	log4javascript.JsonLayout.prototype = new log4javascript.Layout();
	copy(log4javascript.JsonLayout.prototype, {
		setReadable: f,
		isReadable: f
	});

	// HttpPostDataLayout
	log4javascript.HttpPostDataLayout = ff();
	log4javascript.HttpPostDataLayout.prototype = new log4javascript.Layout();

	// PatternLayout
	log4javascript.PatternLayout = ff();
	log4javascript.PatternLayout.prototype = new log4javascript.Layout();

	// NullLayout
	log4javascript.NullLayout = ff();
	log4javascript.NullLayout.prototype = new log4javascript.Layout();

	// Appenders
	log4javascript.Appender = ff();
	log4javascript.Appender.prototype = {
		layout: new log4javascript.PatternLayout(),
		threshold: log4javascript.Level.ALL,
		doAppend: f,
		append: f,
		setLayout: f,
		getLayout: f,
		setThreshold: f,
		getThreshold: f,
		toString: f
	};

	// AlertAppender
	log4javascript.AlertAppender = ff();
	log4javascript.AlertAppender.prototype = new log4javascript.Appender();

	// AlertAppender
	log4javascript.ArrayAppender = ff();
	log4javascript.ArrayAppender.prototype = new log4javascript.Appender();

	// AjaxAppender
	log4javascript.AjaxAppender = ff();
	log4javascript.AjaxAppender.prototype = new log4javascript.Appender();
	copy(log4javascript.AjaxAppender.prototype, {
		isTimed: f,
		setTimed: f,
		getTimerInterval: f,
		setTimerInterval: f,
		isWaitForResponse: f,
		setWaitForResponse: f,
		getBatchSize: f,
		setBatchSize: f,
		setRequestSuccessCallback: f,
		setFailCallback: f,
		sendAll: f,
		defaults: {
			requestSuccessCallback: null,
			failCallback: null
		}
	});

	// ConsoleAppender
	function ConsoleAppender() {}
	ConsoleAppender.prototype = new log4javascript.Appender();
	copy(ConsoleAppender.prototype, {
		create: f,
		isNewestMessageAtTop: f,
		setNewestMessageAtTop: f,
		isScrollToLatestMessage: f,
		setScrollToLatestMessage: f,
		getWidth: f,
		setWidth: f,
		getHeight: f,
		setHeight: f,
		getMaxMessages: f,
		setMaxMessages: f
	});

	// InPageAppender
	log4javascript.InPageAppender = ff();
	log4javascript.InPageAppender.prototype = new ConsoleAppender();
	copy(log4javascript.InPageAppender.prototype, {
		isInitiallyMinimized: f,
		setInitiallyMinimized: f,
		hide: f,
		show: f,
		isVisible: f,
		close: f,
		defaults: {
			layout: new log4javascript.PatternLayout(),
			maxMessages: null
		}
	});
	log4javascript.InlineAppender = log4javascript.InPageAppender;

	// PopUpAppender
	log4javascript.PopUpAppender = ff();
	log4javascript.PopUpAppender.prototype = new ConsoleAppender();
	copy(log4javascript.PopUpAppender.prototype, {
		isUseOldPopUp: f,
		setUseOldPopUp: f,
		isComplainAboutPopUpBlocking: f,
		setComplainAboutPopUpBlocking: f,
		isFocusPopUp: f,
		setFocusPopUp: f,
		isReopenWhenClosed: f,
		setReopenWhenClosed: f,
		close: f,
		defaults: {
			layout: new log4javascript.PatternLayout(),
			maxMessages: null
		}
	});

	// BrowserConsoleAppender
	log4javascript.BrowserConsoleAppender = ff();
	log4javascript.BrowserConsoleAppender.prototype = new log4javascript.Appender();
})();
var log4javascript_dummy = log4javascript;