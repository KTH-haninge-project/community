/*
 * Class:		multipleSelectFilter
 * Author:		Chris Bolson <chris[at]cbolson.com>
 * Version:		1.0
 * Date:		2010-04-04
 * Notes:		Modal Window code adapted from original MooBox code by Zohar Arad:
 *				http://www.zohararad.com/posts/show_post/5
 * 
 * 
 * Use: 		Multiple Select List Filter in Modal Window
 * Description:	Creates a list of a multiple select list options with a text input to filter the results
 * Requires: 	core 1.2.4
 *				more: delegation
 *
 * Initiate: 	new multipleSelectFilter({options})
 * Options:
 * 				zIndex (int) - z-index level for the modal window.
 * 				classes (json) - CSS classes to style the modal window
 * 					- overlay - Page overlay CSS class
 * 					- window - Modal window CSS class
 * 					- content - Modal window content element CSS class
 * 					- closer - Modal window close button CSS class
 * 					- titlebar - Modal window title bar CSS class
 * 				size (json) - Modal window dimensions in default states
 * 					- x
 *					- y
******************************/


var multipleSelectFilter = new Class({
	Implements: [Events, Options, Chain],
	options:{
		zIndex:10000,
		classes:{
			overlay  : 'modal-overlay',
			window   : 'modal-window',
			content  : 'modal-content',
			closer   : 'modal-closer',
			titlebar : 'modal-titlebar'
		},
		size:{x:500,y:250},
		txtInitial:'Type here to search...',
		txtBtClose:'Close',
		txtBtFilterReset:'Reset Filter',
		txtBtListClear:'Unselect All',
		imgClose:'assets/close.png'
	},
	
	// Initializes the modal class
	initialize:function(options){
		this.setOptions(options);
		this.build();
		this.setInitialPosition();
	},
	
	// Builds the modal window components (window, content, title bar, close button, page overlay)
	build:function(){
		
		//title bar (title text inserted according to select filter)
		htmlTitle='<h3></h3>';
		this.titlebar = new Element('div',{
			'class':this.options.classes.titlebar,
			'html':'<h3></h3>'
		});
		
		//	close button
		this.closer = new Element('img',{
			'class':this.options.classes.closer,
			'src':this.options.imgClose,
			'title':this.options.txtBtClose,
			events:{
				'click':this.close.bindWithEvent(this)
			}
		}).inject(this.titlebar,'bottom').setStyle('cursor','pointer');
		
		//	contents holder - the filter is loaded into this element
		this.content = new Element('div',{
			'class':this.options.classes.content
		});
		
		//	the modal window
		this.modal = new Element('div',{
			'class':this.options.classes.window,
			styles:{'display':'none','z-index':this.options.zIndex+1}
		}).adopt(this.titlebar,this.content);
		
		//	overlay
		this.overlay = new Element('div',{
			'html':'&nbsp;',
			'class':this.options.classes.overlay,
			styles:{
				'display':'none',
				'z-index':this.options.zIndex,
				'background':'#000',
				'height':document.getScrollSize().y+'px'
			}
		});
		
		// inject the whole lot into the page
		$(document.body).adopt(this.overlay,this.modal);
		
		// Add an iframe below the content element to fix IE6 select element z-index bug
		if(Browser.Engine.trident4){
			var iframe1 = new Element('iframe',{
				styles:{'position':'absolute','width':'104%','height':'110%','top':'-5%','left':'-2%','z-index':'-1'},
				height:'100%',
				width:'100%',
				frameborder:0
			}).setOpacity(0).inject(this.modal,'top');
			var iframe2 = new Element('iframe',{
				styles:{'position':'absolute','width':'120%','height':'120%','top':'-10%','left':'-10%'},
				height:'100%',
				width:'100%',
				frameborder:0
			}).setOpacity(0).inject(this.overlay,'top');
		}
	},
	
	//	get all the "openers" and add event
	bindOpeners:function(els){
		this.openers = $splat(els);
		if(this.openers.length == 0) {
			return;
		}
		var _this = this;
		this.openers.addEvent('click',function(e){
			var ev = new Event(e);
			ev.preventDefault();
			_this.getContent(this);
		});
	},
	
	//	get filter contents and open modal window
	getContent:function(el){
		this.chain.apply(this,[
				this.filter(el.get('id')),
				this.toggleOverlay(),
				this.open()
			]
		);
	},
	
	//	set content
	setContent:function(modalContent,modalSize,modalTitle){
		// will use this to make the box size according to select list width ? - careful with the buttons!
		var size = $type(modalSize) != 'object' ? this.options.size : modalSize;
		this.modal.getElement('h3').set('text',modalTitle);
		this.content.empty().adopt(modalContent).set('opacity',1);
	},
	
	// toggleOverlay
	toggleOverlay:function(state){
		var opacity = state || this.overlay.retrieve('opacity');
		this.overlay.setStyle('display',opacity ? 'none' : 'block').set('opacity',opacity ? 0 : 0.6);
	},
	
	// Opens the modal window
	open:function(){
		this.setInitialPosition();
		this.modal.set('opacity',1);
		//this.content.set('html','loading...');
	},
	
	// Close the modal window and remove it's contents
	close:function(e){
		if(e){ e.preventDefault(); }
		this.modal.set('opacity',0);
		this.overlay.set('opacity',0);
		this.content.empty();
		if(this.request){ this.request.cancel();}
	},
	
	// getWindowDimentions
	getWindowDimentions:function(size){
		var docSize = document.getSize(), scroll = document.getScroll();
		var params = {
			left: (scroll.x + (docSize.x - size.x) / 2).toInt(),
			right: (scroll.x - (docSize.x - size.x) / 2).toInt(),
			top: (scroll.y + (docSize.y - size.y) / 2).toInt(),
			height:(size.y).toInt(),
			width: (size.x).toInt()
		};
		return params;
	},
	//set initial window position
	setInitialPosition:function(){
		this.modal.setStyles($merge({
			display:'block',
			opacity:0
		},this.getWindowDimentions(this.options.size)));
	},
	
	//	create the select list filter )get options, 
	filter:function(id){
		var selectList		= $(''+id.replace('filter_','')+'');
		var selectOptions	= selectList.getElements('option');
		var title			= selectList.get('title');
		var selectBoxHeight	= (this.options.size.y-80);
		var txtInitial		= this.options.txtInitial;
		
		//	create array select list elements
		var listItems="";
		var filterOptions=selectOptions.each(function(option,index){	
			optionValue=option.get('value').trim();
			if(optionValue!=""){
				var optionClasses='';
				var optionText	=option.get('text');
				
				//	check if option is selected - add class
				if(option.get('selected')) 	optionClasses+=' checked';
				
				//	check if option has special class (used if the basic select list has special classes - these classes must be added to the style sheet
				if(option.get('class')) 	optionClasses+=' '+option.get('class')+'';
				
				//	add option to unordered list
				listItems += '<li id="opt_'+index+'" rel="'+optionValue+'" class="'+optionClasses+'">'+optionText+'</li>';
			}
		});		
		
		//	CREATE FILTER - input, buttons and list
		//	use html string method to optimize for ie6
		var filterHTML='';
		filterHTML+='<input type="text" id="searchbox" class="search" value="'+txtInitial+'">';
		filterHTML+='<input type="button" id="btListClear" class="btFilter" value="'+this.options.txtBtListClear+'">';
		filterHTML+='<input type="button" id="btListReset" class="btFilter" value="'+this.options.txtBtFilterReset+'">';
		//filterHTML+='<input type="button" id="btSave" class="btFilter" value="'+this.options.txtBtClose+'">';
		filterHTML+='<ul style="height:'+selectBoxHeight+'px;">'+listItems+'</ul>';
		
		var filterWrapper = new Element('div',{
			'class':'filterWrapper',
			'html':filterHTML
		});
		
		//	define elements
		var filterList		= filterWrapper.getElement('ul');
		var filterListItems = filterList.getElements('li').setStyle('cursor','pointer');
		//	store item index,value and text
		filterListItems.each(function(el){
			el.store('oTxtValue',el.get('html').toLowerCase()).store('oValue',el.get('rel')).store('oIndex',el.id.replace('opt_',''));
	    });
		
		var filterTextbox	= filterWrapper.getFirst('input').addEvents({
			'focus':function(el){
				if(this.value==''+txtInitial+''){ this.value="";}
			},
			'keyup':function(){
				checkListMatches(this.value);
			}
		});
		
		//	reset filter
		var btReset = filterWrapper.getElement('input[id=btListReset]').addEvent('click',function(){
			filterListReset();
		});
		
		//	clear all selected options
		var btClear = filterWrapper.getElement('input[id=btListClear]').addEvent('click',function(){
			filterClearList();
		});
		
		//	filter for matches
		var checkListMatches = function(str){
	        str=str.toLowerCase();
	        filterListItems.each(function(el){
	            (el.retrieve('oTxtValue').contains(str)) ? el.setStyle('display','block') :el.setStyle('display','none');
	        });
	    };
	    
	    //	reset filter list
	    var filterListReset = function(){
	    	 filterListItems.each(function(el){
	            el.setStyle('display','block');
	            filterTextbox.set('value','');
	        });
	    }
	 	
	 	//	clear selected items
	    var filterClearList=function(){
	    	filterListItems .removeClass('checked');	 
			selectList.getSelected().setProperty('selected', false);
	       	filterListReset();
	    }
	    
		//	add events for li items via delegation method
		filterList.addEvents({
			'click:relay(li)':function(){
				//	define id of item clicked
				index=filterListItems.indexOf(this);
				optionID=filterListItems[index].retrieve('oIndex');
				
				if(filterListItems[index].hasClass('checked')){
					filterListItems[index].removeClass('checked');	
					selectOptions[optionID].setProperty('selected', false);
				}else{
					filterListItems[index].addClass('checked');
					selectOptions[optionID].setProperty('selected', true);
				}
			},
			'mouseover:relay(li)':function(event){
				if(event.shift){
					index=filterListItems.indexOf(this);
					optionID=filterListItems[index].retrieve('oIndex');
					if(filterListItems[index].hasClass('checked')){
			  			filterListItems[index].removeClass('checked');
			  			selectOptions[optionID].setProperty('selected', false);
			  		}else{
			   			filterListItems[index].addClass('checked');
			   			selectOptions[optionID].setProperty('selected', true);
			   		}
				}else{
					this.highlight('#FFCC00',this.getStyle('backgroundColor'));
			   	}
			}
			
		});
		
		//	add contents to modal window
		var size = this.options.size; //	I want to modify this to get the width of the widest text value (careful of the buttons)
		this.setContent(filterWrapper,size,selectList.get('title'));
		
		//	set focus to filter box to start typing
		filterTextbox.focus();
		
	}
});


// Initiate modal window and Select List Filter
window.addEvent('domready',function() {
	
	//	add seach buttons dynamically
	var imgSearch='<img src="assets/icon-search.png" border="0" title="Activate Filter for this search list" alt="search">';
	$$('select[.searchables]').each(function(el,index){
		var idSelect=el.id;
		var lnk=new Element('div',{'html':'<a href="#" rel="filter" id="filter_'+idSelect+'" title="Filter List: '+idSelect+'" class="btSelectFilter">'+imgSearch+'</a>'}).inject(el,'after');
	})
	//	initiate class
	var filter = new multipleSelectFilter();
	
	// bind the links to the modal instance
	filter.bindOpeners($$('a.btSelectFilter'));
});
