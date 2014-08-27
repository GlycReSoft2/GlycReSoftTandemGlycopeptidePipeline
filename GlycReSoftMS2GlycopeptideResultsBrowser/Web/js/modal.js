var ModalInstanceCtrl;

ModalInstanceCtrl = function($scope, $modalInstance, title, items, summary, postLoadFn) {
  $scope.title = title;
  $scope.items = items;
  $scope.summary = summary;
  $scope.postLoadFn = postLoadFn;
  $scope.ok = function() {
    console.log($scope);
    return $modalInstance.close(true);
  };
  return $scope.cancel = function() {
    return $modalInstance.dismiss("cancel");
  };
};
