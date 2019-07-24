/*----------------------------------------------------*/
/*  SCEditor
/*----------------------------------------------------*/

(function($){
	$(document).ready(function(){
		$(".WYSIWYG").sceditor({
			plugins: "bbcode",
			toolbar: "bold,italic,underline,center,right,justify,font,size,color,removeformat,bulletlist,orderedlist,table,quote,image,link,ltr,rtl,source",
			width: "100%"
		});

		function addIng() {
			var newElem = $('tr.ingredients-cont.ing:first').clone();
			newElem.find('input').val('');
			newElem.appendTo('table#ingredients-sort');
		}

		//sortable table
		var fixHelper =  function(e, tr) {
		    var $originals = tr.children();
		    var $helper = tr.clone();
		    $helper.children().each(function(index)
		    {
		      // Set helper cell sizes to match the original sizes
		      $(this).width($originals.eq(index).width());
		    });
		    return $helper;
		  };
		if ($("table#ingredients-sort").is('*')) {
			$('.add_ingredient').click(function(e){
				e.preventDefault();
				addIng();
			})

			$('.add_separator').click(function(e){
				e.preventDefault();
				var newElem = $('<tr class="ingredients-cont separator"><td class="icon"><i class="fa fa-arrows"></i></td><td><input name="ingredient_name" type="text" class="ingredient" placeholder="" /></td><td><input name="ingredient_note" type="text" class="notes" placeholder="Separator" /></td><td class="action"><a title="Delete" class="delete" href="#"><i class="fa fa-remove"></i></a></td></tr>');
				newElem.appendTo('table#ingredients-sort');
			})
			// remove ingredient
			$('#ingredients-sort .delete').live('click',function(e){
				e.preventDefault();
				$(this).parent().parent().remove();
			});

			$('table#ingredients-sort tbody').sortable({
				forcePlaceholderSize: true,
				forceHelperSize: true,
				placeholder : 'sortableHelper',
				helper: fixHelper,
				zIndex: 999990,
				opacity: 0.6,
				tolerance: "pointer"
			})
	 	}

	});
})(this.jQuery);